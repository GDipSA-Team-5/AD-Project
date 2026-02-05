#!/usr/bin/env bash
set -e

# Directory for cached data (NVD, CISA, RetireJS)
DATA_DIR="./dependency-check-data"
mkdir -p "$DATA_DIR"

# Run OWASP Dependency-Check using binary installed in PATH
dependency-check.sh \
  --project ADWebApplication \
  --scan . \
  --format ALL \
  --out ./dependency-check-report \
  --data "$DATA_DIR" \
  --nvdApiKey "${NVD_API_KEY}" \
  --failOnCVSS 9 \
  -n  # disables auto-update, uses cached DB
