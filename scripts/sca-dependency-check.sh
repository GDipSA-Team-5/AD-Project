#!/usr/bin/env bash
set -e

# Run OWASP Dependency-Check
dependency-check.sh \
  --project ADWebApplication \
  --scan . \
  --format ALL \
  --out ./dependency-check-report \
  --failOnCVSS 7 \
  --disableNvd
