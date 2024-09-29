# Use the official Node.js image
FROM node:18

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json (if they exist) to the container
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the entire project into the container (you will override this with volumes during development)
COPY . .

# Expose the port the app runs on
EXPOSE 3000

# Start the Next.js development server
CMD ["npm", "run", "dev"]