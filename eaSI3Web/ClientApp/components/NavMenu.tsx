import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';

export class NavMenu extends React.Component<{}, {}> {
    public render() {

        return <div className=''>


            <div className='main-easi3'>
                <h1 className="main-h1-1">IMPUTA FÁCIL CON...</h1>
                <h1 className="main-h1-2">-EASI3-</h1>
                <h2 className="main-h2">Y disfruta Del Fin De Semana</h2>
                <div className="main-btn">
                <NavLink to={'/'} exact activeClassName='active' id='imputarHoras'>
                    <span className='glyphicon glyphicon-home'></span> Imputar Horas
                </NavLink>
                </div>
                <div className="main-btn">
                    <NavLink to={'/vincular'} exact activeClassName='active' id='vincularTarea'>
                    <span className='glyphicon glyphicon-plus'></span> Vincular Tarea
                </NavLink>
                </div >
                <div className="main-img">
                <img src="http://www.openfinance.es/wp-content/uploads/thegem-logos/logo_1bb293e7e736d552df6e313662c968df_1x.png" />
                </div>
            </div>
        </div>;
    }
}
