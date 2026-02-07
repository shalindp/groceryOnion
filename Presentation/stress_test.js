import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false
};

export default () => {
    const pricingPayload = JSON.stringify({
        "WoolworthStoreIds": [3496448, 2810973, 1906035],
        "PaknSaveStoreIds": [],
        "ProductIdAndStoreSkus": [
            { "ProductId": "ec8863c4-eaaa-4ffa-a500-e2b8a15929e3", "StoreSku": "35285" },
            { "ProductId": "807b5ba4-6ae0-451b-b973-ff2580c791ca", "StoreSku": "726442" },
            { "ProductId": "12c9a06f-d910-4871-9e45-7946b51c1771", "StoreSku": "144607" }
        ]
    });

    const params = {
        headers: { 'Content-Type': 'application/json' },
    };

    // 1 API call per scroll
    http.post("http://192.168.0.100:5112/product/price", pricingPayload, params);

    // Random delay between scrolls (1s - 1.5s) to simulate human scrolling
    sleep(Math.random() * 0.5 + 1);
};
