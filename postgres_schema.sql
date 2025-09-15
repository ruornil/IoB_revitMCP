-- Schema updated for multi-document safety (composite keys)

-- Table: revit_elements
-- V2 schema prefers composite keys including doc_id to support multiple models
-- Clean installs should use this section; existing DBs created with older schema
-- will still work due to command fallbacks in code.
CREATE TABLE IF NOT EXISTS revit_elements (
    id INTEGER NOT NULL,
    guid UUID,
    name TEXT,
    category TEXT,
    type_name TEXT,
    level TEXT,
    doc_id TEXT NOT NULL,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_revit_elements PRIMARY KEY (doc_id, id)
);


-- Table: model_info
CREATE TABLE IF NOT EXISTS model_info (
    doc_id TEXT PRIMARY KEY,
    model_name TEXT,
    guid UUID,
    last_saved TIMESTAMP,
    project_info JSONB,
    project_parameters JSONB
);

-- Table: revit_elementTypes
CREATE TABLE IF NOT EXISTS revit_elementTypes (
    id INTEGER NOT NULL,
    guid UUID,
    family TEXT,
    type_name TEXT,
    category TEXT,
    doc_id TEXT NOT NULL,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_revit_elementtypes PRIMARY KEY (doc_id, id)
);

-- Table: revit_parameters
CREATE TABLE IF NOT EXISTS revit_parameters (
    id SERIAL PRIMARY KEY,
    doc_id TEXT,
    element_id INTEGER,
    param_name TEXT,
    param_value TEXT,
    is_type BOOLEAN,
    applicable_categories TEXT[],
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_element_param_v2 UNIQUE (doc_id, element_id, param_name)
);

CREATE INDEX IF NOT EXISTS idx_revit_parameters_doc_param ON revit_parameters (doc_id, param_name);
CREATE INDEX IF NOT EXISTS idx_revit_parameters_doc_element ON revit_parameters (doc_id, element_id);

CREATE TABLE IF NOT EXISTS revit_type_parameters (
    id SERIAL PRIMARY KEY,
    doc_id TEXT,
    element_type_id INTEGER,
    param_name TEXT,
    param_value TEXT,
    applicable_categories TEXT[],
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_type_param_v2 UNIQUE (doc_id, element_type_id, param_name)
);

CREATE INDEX IF NOT EXISTS idx_revit_type_parameters_doc_param ON revit_type_parameters (doc_id, param_name);
CREATE INDEX IF NOT EXISTS idx_revit_type_parameters_doc_type ON revit_type_parameters (doc_id, element_type_id);

-- Table: revit_categories
CREATE TABLE IF NOT EXISTS revit_categories (
    id SERIAL PRIMARY KEY,
    enum TEXT,
    name TEXT,
    category_group TEXT,
    description TEXT,
    guid UUID,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_category_enum UNIQUE (enum)
);

-- Table: revit_views
CREATE TABLE IF NOT EXISTS revit_views (
    id INTEGER NOT NULL,
    guid UUID,
    name TEXT,
    view_type TEXT,
    scale INTEGER,
    discipline TEXT,
    detail_level TEXT,
    associated_sheet_id INTEGER,
    doc_id TEXT NOT NULL,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_revit_views PRIMARY KEY (doc_id, id)
);

-- Table: revit_sheets
CREATE TABLE IF NOT EXISTS revit_sheets (
    id INTEGER NOT NULL,
    guid UUID,
    name TEXT,
    number TEXT,
    title_block TEXT,
    doc_id TEXT NOT NULL,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_revit_sheets PRIMARY KEY (doc_id, id)
);

-- Table: revit_schedules
CREATE TABLE IF NOT EXISTS revit_schedules (
    id INTEGER NOT NULL,
    guid UUID,
    name TEXT,
    category TEXT,
    doc_id TEXT NOT NULL,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_revit_schedules PRIMARY KEY (doc_id, id)
);

-- Table: revit_families
CREATE TABLE IF NOT EXISTS revit_families (
    id SERIAL PRIMARY KEY,
    name TEXT,
    family_type TEXT,
    category TEXT,
    guid UUID,
    doc_id TEXT,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_family_name_type_v2 UNIQUE (doc_id, name, family_type, category)
);

-- Table: mcp_queue
CREATE TABLE IF NOT EXISTS mcp_queue (
    id SERIAL PRIMARY KEY,
    plan JSONB,
    status TEXT DEFAULT 'pending',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP,
    result JSONB
);

-- Optional: Revit link instances (created automatically by code if missing)
CREATE TABLE IF NOT EXISTS revit_link_instances (
  host_doc_id TEXT NOT NULL,
  instance_id INTEGER NOT NULL,
  link_doc_id TEXT NOT NULL,
  origin_x DOUBLE PRECISION,
  origin_y DOUBLE PRECISION,
  origin_z DOUBLE PRECISION,
  basisx_x DOUBLE PRECISION,
  basisx_y DOUBLE PRECISION,
  basisx_z DOUBLE PRECISION,
  basisy_x DOUBLE PRECISION,
  basisy_y DOUBLE PRECISION,
  basisy_z DOUBLE PRECISION,
  basisz_x DOUBLE PRECISION,
  basisz_y DOUBLE PRECISION,
  basisz_z DOUBLE PRECISION,
  rotation_z_radians DOUBLE PRECISION,
  angle_to_true_north_radians DOUBLE PRECISION,
  last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT pk_revit_link_instances PRIMARY KEY (host_doc_id, instance_id)
);

-- Linked elements per link instance (duplicates of the same link doc elements are
-- captured per-instance under the host model context)
CREATE TABLE IF NOT EXISTS revit_linked_elements (
  host_doc_id TEXT NOT NULL,
  link_instance_id INTEGER NOT NULL,
  link_doc_id TEXT NOT NULL,
  id INTEGER NOT NULL,
  guid UUID,
  name TEXT,
  category TEXT,
  type_name TEXT,
  level TEXT,
  last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT pk_revit_linked_elements PRIMARY KEY (host_doc_id, link_instance_id, id)
);

-- Optional: parameters for linked elements (not required if only metadata needed)
CREATE TABLE IF NOT EXISTS revit_linked_parameters (
  id SERIAL PRIMARY KEY,
  host_doc_id TEXT NOT NULL,
  link_instance_id INTEGER NOT NULL,
  element_id INTEGER NOT NULL,
  param_name TEXT,
  param_value TEXT,
  is_type BOOLEAN,
  applicable_categories TEXT[],
  last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT uq_revit_linked_parameters UNIQUE (host_doc_id, link_instance_id, element_id, param_name)
);

-- Linked element types per link instance
CREATE TABLE IF NOT EXISTS revit_linked_elementtypes (
  host_doc_id TEXT NOT NULL,
  link_instance_id INTEGER NOT NULL,
  link_doc_id TEXT NOT NULL,
  id INTEGER NOT NULL,
  guid UUID,
  family TEXT,
  type_name TEXT,
  category TEXT,
  last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT pk_revit_linked_elementtypes PRIMARY KEY (host_doc_id, link_instance_id, id)
);

-- Optional: per-host model info for a linked document
CREATE TABLE IF NOT EXISTS model_info_linked (
  host_doc_id TEXT NOT NULL,
  link_doc_id TEXT NOT NULL,
  model_name TEXT,
  guid UUID,
  last_saved TIMESTAMP,
  project_info JSONB,
  project_parameters JSONB,
  CONSTRAINT pk_model_info_linked PRIMARY KEY (host_doc_id, link_doc_id)
);

-- UI events for dashboard insights
CREATE TABLE IF NOT EXISTS ui_events (
  id SERIAL PRIMARY KEY,
  session_id TEXT,
  doc_id TEXT,
  event_type TEXT NOT NULL,
  payload JSONB,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
