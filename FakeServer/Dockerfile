# Use the latest Node.js image
FROM node:latest

# Set the working directory inside the container
WORKDIR /app

# Create a package.json file
RUN npm init -y

# Install json-server@0.17.4
RUN npm install -g json-server@0.17.4

# Copy the rest of the application code
COPY . .

# Expose the port that json-server will run on
EXPOSE 3000

# Command to run json-server
CMD ["json-server", "--watch", "db.json", "--routes", "routes.json"]
# CMD ["json-server", "--version"]