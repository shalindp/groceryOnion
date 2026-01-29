import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 10,
    duration: '10s'
}

export default ()=>{
    // http.post("http://localhost:5112/product/pricing/by/region")
    http.get("http://localhost:5112/product/searchV2?term=milk&itemsPerPage=10&pageNumber=1")
}
