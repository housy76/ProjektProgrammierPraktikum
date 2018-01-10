#!/bin/bash

echo "importiere datenbank von: /home/vinf02/exportDB/backupDB.sql"

mysql -u root --password=test_1234 -e "create database verwaltung;"
mysql -u root --password=test_1234 verwaltung < /home/vinf02/exportDB/backupDB.sql
