const PROXY_CONFIG = [
    {
        context: [
            "/api",
            "/uploads"
        ],
        target: "http://localhost:6000",
        secure: false
    },
    {
        context: [
            "/avatars"
        ],
        target: "http://localhost:5000",
        secure: false
    }
];

module.exports = PROXY_CONFIG;