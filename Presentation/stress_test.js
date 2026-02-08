import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false
};

export default () => {
    const pricingPayload = JSON.stringify([
        {
            "ProductId": "ec8863c4-eaaa-4ffa-a500-e2b8a15929e3",
            "StoreName": 2,
            "StoreId": "3496448",
            "StoreSku": "35285"
        },
        {
            "ProductId": "ec8863c4-eaaa-4ffa-a500-e2b8a15929e3",
            "StoreName": 2,
            "StoreId": "3496448",
            "StoreSku": "35285"
        },
        {
            "ProductId": "ec8863c4-eaaa-4ffa-a500-e2b8a15929e3",
            "StoreName": 2,
            "StoreId": "3496448",
            "StoreSku": "35285"
        },
        // {
        //     "ProductId": "807b5ba4-6ae0-451b-b973-ff2580c791ca",
        //     "StoreName": 2,
        //     "StoreId": "2810973",
        //     "StoreSku": "726442"
        // },
        // {
        //     "ProductId": "807b5ba4-6ae0-451b-b973-ff2580c791ca",
        //     "StoreName": 2,
        //     "StoreId": "2810973",
        //     "StoreSku": "726442"
        // },
        // {
        //     "ProductId": "807b5ba4-6ae0-451b-b973-ff2580c791ca",
        //     "StoreName": 2,
        //     "StoreId": "2810973",
        //     "StoreSku": "726442"
        // },
        // {
        //     "ProductId": "12c9a06f-d910-4871-9e45-7946b51c1771",
        //     "StoreName": 2,
        //     "StoreId": "1906035",
        //     "StoreSku": "144607"
        // },
        // {
        //     "ProductId": "12c9a06f-d910-4871-9e45-7946b51c1771",
        //     "StoreName": 2,
        //     "StoreId": "1906035",
        //     "StoreSku": "144607"
        // },
        // {
        //     "ProductId": "12c9a06f-d910-4871-9e45-7946b51c1771",
        //     "StoreName": 2,
        //     "StoreId": "1906035",
        //     "StoreSku": "144607"
        // },
        {
            "ProductId": "2c0d564b-53aa-4b40-b07f-eee5e8b49224",
            "StoreName": 0,
            "StoreId": "3404c253-577f-45ca-b301-c98312e46efb",
            "StoreSku": "5019876-EA-000"
        },
        // {
        //     "ProductId": "c70dbfbd-858e-4e02-8aa3-d818a147a49f",
        //     "StoreName": 0,
        //     "StoreId": "3404c253-577f-45ca-b301-c98312e46efb",
        //     "StoreSku": "5013751-EA-000"
        // },
        // {
        //     "ProductId": "7fbedf17-17a7-4d4e-9d01-5e008dbbfb49",
        //     "StoreName": 0,
        //     "StoreId": "3404c253-577f-45ca-b301-c98312e46efb",
        //     "StoreSku": "5241937-EA-000"
        // }
    ]);

    const params = {
        headers: { 'Content-Type': 'application/json' },
    };

    // 1 API call per scroll
    http.post("http://192.168.0.100:5112/product/price", pricingPayload, params);

    // Random delay between scrolls (1s - 1.5s) to simulate human scrolling
    sleep(Math.random() * 0.5 + 1);
};
