


export default 

function FormData(props){

const styles = {width:"100px", height:"50px", marginLeft:"20px",backgroundColor:"white"}




    return (

        <form>

            <button style={styles}  value="login" onClick={e=>{e.preventDefault(); props.getLogin(props.mgr)}} > LOGIN </button>
            <button style={styles}   value="api" onClick={e=>{e.preventDefault(); props.getAPI(props.mgr)}}>  API </button>
            <button style={styles}  value="logout" onClick={e=>{e.preventDefault(); props.getLogout(props.mgr)}} > LOGOUT </button>

        </form>
    )
}




