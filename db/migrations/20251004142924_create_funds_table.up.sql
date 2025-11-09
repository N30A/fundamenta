CREATE TABLE IF NOT EXISTS funds (
    instrument_id INT PRIMARY KEY REFERENCES instruments(id),
    holdings_url TEXT
); 
