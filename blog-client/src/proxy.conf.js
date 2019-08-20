const PROXY_CONFIG = [
    {
        context: [
            "/api"
        ],
        target: "http://localhost:6000",
        secure: false
    }
    //,
    // {
    //     context: [
    //         "/test"
    //     ],
    //     target: "https://localhost:1111",
    //     secure: true
    // }
];

module.exports = PROXY_CONFIG;