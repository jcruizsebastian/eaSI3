import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { VincularTarea } from './components/VincularTarea';
import { LoginGeneral } from './components/LoginGeneral';

export const routes = <Layout>
    <Route exact path='/' render={() => <Home />} />
    <Route exact path="/vincular" render={() => <VincularTarea jiraKey="" vincular={() => { }} />} />
    
</Layout>;
