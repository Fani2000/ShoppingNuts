#!/bin/sh

# Generate env-config.js with runtime environment variables
echo "console.log('Environment Config Loaded');" > /usr/share/nginx/html/env-config.js
echo "window.ENV = {" >> /usr/share/nginx/html/env-config.js

# Add BackendApi__Url
if [ -n "$BackendApi__Url" ]; then
  echo "  BackendApi__Url: '$BackendApi__Url'," >> /usr/share/nginx/html/env-config.js
else
  echo "  BackendApi__Url: 'http://backend:8080'," >> /usr/share/nginx/html/env-config.js
fi

# Add ASPNETCORE_ENVIRONMENT
if [ -n "$ASPNETCORE_ENVIRONMENT" ]; then
  echo "  ASPNETCORE_ENVIRONMENT: '$ASPNETCORE_ENVIRONMENT'," >> /usr/share/nginx/html/env-config.js
else
  echo "  ASPNETCORE_ENVIRONMENT: 'Production'," >> /usr/share/nginx/html/env-config.js
fi

# Close the ENV object
echo "};" >> /usr/share/nginx/html/env-config.js

# Start the main process
exec "$@"