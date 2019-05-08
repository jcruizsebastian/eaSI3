var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
import * as React from "react";
var Cube = /** @class */ (function (_super) {
    __extends(Cube, _super);
    function Cube() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Cube.prototype.render = function () {
        return React.createElement("div", { className: "wrap" },
            React.createElement("div", { className: "cube" },
                React.createElement("div", { className: "front" },
                    React.createElement("img", { width: "140px", className: "imgCubeBme", src: "https://www.bolsasymercados.es/images/Base/Logo.svg" })),
                React.createElement("div", { className: "back" },
                    React.createElement("img", { width: "140px", className: "imgCubeBme", src: "https://www.bolsasymercados.es/images/Base/Logo.svg" })),
                React.createElement("div", { className: "top" }),
                React.createElement("div", { className: "bottom" }),
                React.createElement("div", { className: "left" },
                    React.createElement("img", { width: "140px", className: "imgCubeOpen", src: "http://www.openfinance.es/wp-content/uploads/thegem-logos/logo_1bb293e7e736d552df6e313662c968df_1x.png" })),
                React.createElement("div", { className: "right" },
                    React.createElement("img", { width: "140px", className: "imgCubeOpen", src: "http://www.openfinance.es/wp-content/uploads/thegem-logos/logo_1bb293e7e736d552df6e313662c968df_1x.png" }))));
    };
    return Cube;
}(React.Component));
export { Cube };
//# sourceMappingURL=Cube.js.map