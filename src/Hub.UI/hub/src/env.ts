import { z } from 'zod';

const envSchema = z.object({
  APP_URL: z.string().default('http://localhost:3000'),
  API_URL: z.string().default('http://localhost:5155/api'),
});

export default envSchema.parse(process.env);
