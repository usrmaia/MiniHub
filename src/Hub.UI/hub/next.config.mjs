/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: false,
  env: {
    APP_URL: process.env.APP_URL,
    API_URL: process.env.API_URL,
  },
};

export default nextConfig;
