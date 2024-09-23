# mealio

Currently a single API endpoint for processing recipe images into JSON to interact with other deployed services that generate meal plans from a repository of recipes

Explanation and background..

## Local development

### Configuration

1. Get an OpenAi Api key
   - You will need to register for an OpenAI account and generate a key if you don’t already have one.

#### Docker

If running in Docker a `.env` file can be created in the project root.

Here’s an overview of the variables and what they are for:

- **OpenAi:ApiKey:** The API key for accessing OpenAI services. _(required if using OpenAI)_
  - Example: sk-xxxxxxxxxxxxxxxxxxxxxx

---

1. set this key locally either using `user-secrets` or in environment variables or in a `.env` that you create in the project root.

to set the `user-secrets` key locally:

Open a terminal:

1.  `cd RecipeProcessing.Api`
2.  `dotnet user-secrets init`
3.  `dotnet user-secrets set "OpenAi:ApiKey" "<your-api-key>"`

2. Environment Variables
1. Create a `.env` file in the project root


### Future work

- Remove or make OpenAI integration optional - replace with LLM from HuggingFace
- Expand micro
