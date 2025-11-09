CREATE TABLE IF NOT EXISTS instruments_holdings (
    instrument_id INT REFERENCES instruments(id) ON DELETE CASCADE,
    holding_id INT REFERENCES instruments(id) ON DELETE CASCADE,
    weight DECIMAL(5,2),
    shares INT,
    PRIMARY KEY (instrument_id, holding_id)
);
