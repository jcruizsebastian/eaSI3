import * as React from "react";
import '../css/logingeneral.css';

export class LoginGeneral extends React.Component<{}, {}> {
    constructor(props: any) {
        super(props);
    }
    render() {
        return (
            <body className="bodyLogin">
            <form className="formulario">
                <div><h2>Login</h2></div>
                <div><label>Usuario : <input type="text" name="name"/></label></div>
                <div><label>Contraseña : <input type="password" name="name" /></label></div>
                <div><input type="submit" name="submit" value="Iniciar Sesión"/></div>
                </form>
            </body>
        )
    }
}