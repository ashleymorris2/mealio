# meal-planner

The readme is very much a work in progress

Explanation and background..

## Local development

### Configuration

1. Get an OpenAi Api key
   - You will need to register for an OpenAI account and generate a key if you don’t already have one.

#### Docker

If running in Docker a `.env` file can be created in the project root.

Here’s an overview of the variables and what they are for:

- **COMPOSE_PROJECT_NAME:** The name of the project when using Docker Compose. _(optional)_
  - Example: `myproject`
- **OpenAi:ApiKey:** The API key for accessing OpenAI services. _(required if using OpenAI)_
  - Example: sk-xxxxxxxxxxxxxxxxxxxxxx

---

1. set this key locally either using `user-secrets` or in environment variables.

to set the `user-secrets` key locally:

Open a terminal:

1.  `cd RecipeProcessing.Api`
2.  `dotnet user-secrets init`
3.  `dotnet user-secrets set "OpenAi:ApiKey" "<your-api-key>"`

### Future work

- Remove or make OpenAI integration optional.
