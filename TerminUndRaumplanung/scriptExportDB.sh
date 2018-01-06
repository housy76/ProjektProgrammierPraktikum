#!/bin/bash

echo "exportiere datenbank nach: /home/vinf02/exportDB/backupDB.sql"

mysqldump -u root --password=test_1234 verwaltung > /home/vinf02/exportDB/backupDB.sql
