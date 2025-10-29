CREATE TABLE IF NOT EXISTS replication_methods (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name TEXT UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS distribution_policy (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name TEXT UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS etfs (
    instrument_id INT PRIMARY KEY REFERENCES instruments(id) ON DELETE CASCADE,
    inception_date DATE,
    expense_ratio DECIMAL(5,2),
    transaction_cost DECIMAL(5,2),
    total_assets BIGINT,
    domicile TEXT,
    replication_method INT REFERENCES replication_methods(id) ON DELETE SET NULL,
    distribution_policy INT REFERENCES distribution_policy(id) ON DELETE SET NULL,
    distribution_frequency TEXT
);

CREATE TABLE asset_classes (
   id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
   name TEXT UNIQUE NOT NULL
);