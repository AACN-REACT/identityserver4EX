
import FormData from './form.js';
import Results from './results.js'

import Oidc, { OidcClient } from 'oidc-client'

// this is the config  object 
var config = {
    authority: "http://localhost:5000",
    client_id: "js",
    redirect_uri: "http://localhost:5003/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri : "http://localhost:5003/index.html",
};

//
var mgr = new Oidc.UserManager(config)

// this is the logging funciton 
function log(...args) {
 var string =""
  args.forEach(function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        return  string+= msg + '\r\n';
    }
    
    
    );
}


//these are our event handlers

function login(mgr) {
    mgr.signinRedirect();
}

function api(mgr) {
    mgr.getUser().then(function (user) {
        var url = "http://localhost:5001/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout(mgr) {
    mgr.signoutRedirect();
}

// this is our outter app
function App({mgr, login, api, logout}){

    const [user, setUser] = React.useState(null);
 
 React.useEffect(()=>{

mgr.getUser().then(logged=>logged?setUser(s=>logged):setUser(s=>"no user"))

    },[mgr])
    console.log(user+"<<<--")

    return (
            <>
            <h1>Hello</h1>
            <FormData mgr={mgr} getLogin={login} getAPI={api} getLogout={logout} />
            <Results />
            </>

    )
}






ReactDOM.render(<App mgr={mgr} login={login} api={api} logout={logout} />, document.getElementById('root'))