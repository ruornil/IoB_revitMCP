# Linked Documents: Schema Update and Migration Plan (Draft)

This document proposes DB changes to support linked Revit docs and multiple link instances per host.

## Schema Changes (v2)

- revit_elements: primary key → (doc_id, id)
- revit_elementTypes: primary key → (doc_id, id)
- revit_parameters: add column `doc_id TEXT NOT NULL`; unique → (doc_id, element_id, param_name); FK → revit_elements(doc_id, id)
- revit_type_parameters: add column `doc_id TEXT NOT NULL`; unique → (doc_id, element_type_id, param_name)
- revit_families: recommended to add `doc_id` and unique (doc_id, name, family_type, category)
- New table: revit_link_instances (per host link instance metadata and transform)

Example link instances table:

```psql
CREATE TABLE IF NOT EXISTS revit_link_instances (
  host_doc_id TEXT NOT NULL,
  instance_id INTEGER PRIMARY KEY,
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
  last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

## Incremental Migration Steps

- Add new columns and backfill

```psql
ALTER TABLE revit_parameters ADD COLUMN IF NOT EXISTS doc_id TEXT;
UPDATE revit_parameters p SET doc_id = e.doc_id FROM revit_elements e WHERE p.element_id = e.id AND p.doc_id IS NULL;
ALTER TABLE revit_parameters ALTER COLUMN doc_id SET NOT NULL;

ALTER TABLE revit_type_parameters ADD COLUMN IF NOT EXISTS doc_id TEXT;
UPDATE revit_type_parameters tp SET doc_id = t.doc_id FROM revit_elementTypes t WHERE tp.element_type_id = t.id AND tp.doc_id IS NULL;
ALTER TABLE revit_type_parameters ALTER COLUMN doc_id SET NOT NULL;
```

- Create new uniques (and later FKs)

```psql
ALTER TABLE revit_parameters DROP CONSTRAINT IF EXISTS unique_element_param;
ALTER TABLE revit_parameters ADD CONSTRAINT unique_element_param_v2 UNIQUE (doc_id, element_id, param_name);

ALTER TABLE revit_type_parameters DROP CONSTRAINT IF EXISTS unique_type_param;
ALTER TABLE revit_type_parameters ADD CONSTRAINT unique_type_param_v2 UNIQUE (doc_id, element_type_id, param_name);
```

- Switch base table PKs

```psql
ALTER TABLE revit_elements DROP CONSTRAINT IF EXISTS revit_elements_pkey;
ALTER TABLE revit_elements ADD CONSTRAINT revit_elements_pk_v2 PRIMARY KEY (doc_id, id);

ALTER TABLE revit_elementTypes DROP CONSTRAINT IF EXISTS revit_elementtypes_pkey;
ALTER TABLE revit_elementTypes ADD CONSTRAINT revit_elementtypes_pk_v2 PRIMARY KEY (doc_id, id);
```

- Add composite FKs and link instances table

```psql
ALTER TABLE revit_parameters DROP CONSTRAINT IF EXISTS revit_parameters_element_id_fkey;
ALTER TABLE revit_parameters ADD CONSTRAINT revit_parameters_element_fk_v2 FOREIGN KEY (doc_id, element_id)
  REFERENCES revit_elements (doc_id, id) ON DELETE CASCADE;

CREATE TABLE IF NOT EXISTS revit_link_instances (
  host_doc_id TEXT NOT NULL,
  instance_id INTEGER PRIMARY KEY,
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
  last_saved TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

## Follow‑up Code Changes

- Update upserts in `Data/PostgresDb.cs` and `Data/BatchedPostgresDb.cs` to include `doc_id` in conflict targets and param uniques.
- Add an upsert for `revit_link_instances` and capture transforms, angles during sync.
- Extend `SyncModelToSql` with `sync_links: true` to sync link docs and instance metadata.

## Querying with Instances (example)

```psql
SELECT li.instance_id, e.id AS element_id, e.category, li.origin_x, li.origin_y
FROM revit_link_instances li
JOIN revit_elements e ON e.doc_id = li.link_doc_id
WHERE li.host_doc_id = @host AND e.category = 'Walls';
```

You’re welcome! Happy to continue tomorrow.

Quick recap

Added ListLinkedDocuments and include_linked support for elements, categories, views, and families.
Cleaned up secrets handling and docs, added workflow docs.
Drafted a schema migration plan for link-instance-aware persistence.
Next time, we can:

Implement revit_link_instances upserts and a minimal sync for link instances.
Move DB upserts to composite keys and wire “sync_links”.
Have a great rest of your day!
