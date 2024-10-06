/** @type {import('next').NextConfig} */
const nextConfig = {
    logging: {
        level: 'info',
        fetches: {
            fullUrl:  true,
        }
    }
};

export default nextConfig;
