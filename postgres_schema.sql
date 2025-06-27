-- Table: revit_elements
CREATE TABLE IF NOT EXISTS revit_elements (
    id INTEGER PRIMARY KEY,
    guid UUID,
    name TEXT,
    category TEXT,
    type_name TEXT,
    level TEXT,
    doc_id TEXT,
    last_seen TIMESTAMP DEFAULT CURRENT_TIMESTAMP
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
    id INTEGER PRIMARY KEY,
    guid UUID,
    family TEXT,
    type_name TEXT,
    category TEXT,
    doc_id TEXT,
    last_seen TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: revit_parameters
CREATE TABLE IF NOT EXISTS revit_parameters (
    id SERIAL PRIMARY KEY,
    element_id INTEGER REFERENCES revit_elements(id) ON DELETE CASCADE,
    param_name TEXT,
    param_value TEXT,
    is_type BOOLEAN,
    applicable_categories TEXT[],
    CONSTRAINT unique_element_param UNIQUE (element_id, param_name)
);

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
    id INTEGER PRIMARY KEY,
    guid UUID,
    name TEXT,
    view_type TEXT,
    scale INTEGER,
    discipline TEXT,
    detail_level TEXT,
    associated_sheet_id INTEGER,
    doc_id TEXT,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: revit_sheets
CREATE TABLE IF NOT EXISTS revit_sheets (
    id INTEGER PRIMARY KEY,
    guid UUID,
    name TEXT,
    number TEXT,
    title_block TEXT,
    doc_id TEXT,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: revit_schedules
CREATE TABLE IF NOT EXISTS revit_schedules (
    id INTEGER PRIMARY KEY,
    guid UUID,
    name TEXT,
    category TEXT,
    doc_id TEXT,
    last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP
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
    CONSTRAINT unique_family_name_type UNIQUE (name, family_type, category)
);
