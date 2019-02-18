import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { VincularTarea } from './components/VincularTarea';

export const routes = <Layout>
    <Route exact path='/' component={Home} />
    <Route exact path="/vincular" component={VincularTarea} />
</Layout>;
