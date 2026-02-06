import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 10,
    duration: '10s'
}

export default () => {
    // Stress test for SearchProducts endpoint
    // http.get("http://localhost:5112/product/searchV2?term=milk&itemsPerPage=10&pageNumber=1")

    // Stress test for ProductPriceAsync endpoint
    const pricingPayload = JSON.stringify({
        "WoolworthStoreIds": [3496448, 2810973, 1906035, 1203274],
        "PaknSaveStoreIds": [],
        "ProductIdAndStoreSkus": [{
            "ProductId": "ec8863c4-eaaa-4ffa-a500-e2b8a15929e3",
            "StoreSku": "35285"
        }]
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    http.post("http://192.168.0.100:5112/product/price", pricingPayload, params);
}
