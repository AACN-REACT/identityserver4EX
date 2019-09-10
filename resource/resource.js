const express = require('express');

const path = require('path');


const app = express();

const port = 3000;

console.log("this server is working ")
console.log("hhh")

app.use(express.static('client'))
app.get('/',(req,res)=>{

    res.send("hello thereee")
})


app.listen(port, ()=>console.log(`listening of port ${port}`))


