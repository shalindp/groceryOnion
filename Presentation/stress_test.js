import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 10,
    duration: '10s'
}

export default ()=>{
    http.post("http://localhost:5112/product/pricing/by/region")
}
