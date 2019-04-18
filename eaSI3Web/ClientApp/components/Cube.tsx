import * as React from "react";

export class Cube extends React.Component<{}, {}> {
    public render() {

        return <div className="wrap">
            <div className="cube">
                <div className="front"><img width="140px" className="imgCubeBme" src="https://www.bolsasymercados.es/images/Base/Logo.svg" /></div>
                <div className="back"><img width="140px" className="imgCubeBme" src="https://www.bolsasymercados.es/images/Base/Logo.svg" /></div>
                <div className="top"></div>
                <div className="bottom"></div>
                <div className="left"><img width="140px" className="imgCubeOpen" src="http://www.openfinance.es/wp-content/uploads/thegem-logos/logo_1bb293e7e736d552df6e313662c968df_1x.png" /></div>
                <div className="right"><img width="140px" className="imgCubeOpen" src="http://www.openfinance.es/wp-content/uploads/thegem-logos/logo_1bb293e7e736d552df6e313662c968df_1x.png" /></div>
            </div>
        </div>
    }
}