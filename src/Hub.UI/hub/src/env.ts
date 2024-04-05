import { z } from 'zod';

const envSchema = z.object({
  NODE_ENV: z.string().default(process.env.NODE_ENV),
  APP_URL: z.string().default('http://localhost:3000'),
  API_URL: z.string().default('http://localhost:5155/api'),
});

export default envSchema.parse(process.env);
