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
import * as React from 'react';
import { Link } from 'react-router-dom';
var NavMenu = /** @class */ (function (_super) {
    __extends(NavMenu, _super);
    function NavMenu() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    NavMenu.prototype.render = function () {
        return React.createElement("div", { className: '' },
            React.createElement("div", { className: 'main-easi3' },
                React.createElement("a", { className: "link-confluence", target: "_blank", href: "https://confluence.openfinance.es/display/OP/Manual+eaSI3" },
                    React.createElement("img", { src: "informacio.png", width: "45px", height: "auto" }),
                    React.createElement("span", { className: "tooltiptext-confluence" }, 'Guía de Confluence')),
                React.createElement("h1", { className: "main-h1-1" }, 'IMPUTA FÁCIL CON...'),
                React.createElement("h1", { className: "main-h1-2" }, "-EASI3-"),
                React.createElement("h2", { className: "main-h2" }, "Y disfruta del fin de semana"),
                React.createElement("div", { className: "main-btn" },
                    React.createElement(Link, { to: '/home', id: 'imputarHoras' },
                        React.createElement("span", { className: 'glyphicon glyphicon-open' }),
                        " Imputar Horas")),
                React.createElement("div", { className: "main-btn" },
                    React.createElement(Link, { to: '/vincular', id: 'vincularTarea' },
                        React.createElement("span", { className: 'glyphicon glyphicon-plus' }),
                        " Vincular Tarea"))));
    };
    return NavMenu;
}(React.Component));
export { NavMenu };
//# sourceMappingURL=NavMenu.js.map