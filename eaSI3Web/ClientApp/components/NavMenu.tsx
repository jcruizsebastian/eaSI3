import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import * as ReactDOM from 'react-dom';

export class NavMenu extends React.Component<{}, {}> {


    public render() {

        return <div className=''>


            <div className='main-easi3'>
                <a className="link-confluence" target="_blank" href="https://confluence.openfinance.es/display/OP/Manual+eaSI3">
                    <img src="informacio.png" width="45px" height="auto" />
                    <span className="tooltiptext-confluence">{'Guía de Confluence'}</span>
                </a>
                    <h1 className="main-h1-1">{'IMPUTA FÁCIL CON...'}</h1>
                <h1 className="main-h1-2">-EASI3-</h1>
                <h2 className="main-h2">Y disfruta del fin de semana</h2>
                <div className="main-btn">
                    <Link to={'/home'}  id='imputarHoras' >
                        <span className='glyphicon glyphicon-open'></span> Imputar Horas
                    </Link>
                </div>
                <div className="main-btn">
                    <Link to={'/vincular'}  id='vincularTarea'>
                        <span className='glyphicon glyphicon-plus'></span> Vincular Tarea
                    </Link>
                </div >
            </div>
        </div>;
    }
}
