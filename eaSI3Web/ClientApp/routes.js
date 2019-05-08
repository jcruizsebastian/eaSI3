import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { NavMenu } from './components/NavMenu';
import { VincularTarea } from './components/VincularTarea';
export var routes = React.createElement(Layout, null,
    React.createElement(Route, { exact: true, path: '/', render: function () { return React.createElement(NavMenu, null); } }),
    React.createElement(Route, { exact: true, path: '/home', render: function () { return React.createElement(Home, null); } }),
    React.createElement(Route, { exact: true, path: "/vincular", render: function () { return React.createElement(VincularTarea, { jiraKey: "", vincular: function () { } }); } }));
//# sourceMappingURL=routes.js.map