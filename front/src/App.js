import React, { useEffect } from 'react'
import { createStore, applyMiddleware, combineReducers, compose } from 'redux'
import { BrowserRouter, Redirect, Route, Switch } from 'react-router-dom'
import { Provider, useSelector, useDispatch, shallowEqual } from 'react-redux'
import { ToastProvider } from 'react-toast-notifications' 
import Layout from './components/Layout'
import dataReducer from './reducers/data'
import userReducer from './reducers/user'
import layoutReducer, { setRedirect } from './reducers/layout'
import thunk from 'redux-thunk'
import Inbox from './pages/Inbox'
import Outbox from './pages/Outbox'
import Authorization from './components/Authorization'
import Task from './components/TaskModal'

function App() {
  const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

  const store = createStore(
    combineReducers({
      data: dataReducer,
      user: userReducer,
      layout: layoutReducer,
    }),
    composeEnhancers(applyMiddleware(thunk))
  )
  
  return (
    <Provider store={store}>
      <BrowserRouter>
        <ToastProvider>
          <Layout>
            <Authorization>
              <AppRedirect />
              <Switch>
                <Route path='/inbox'>
                  <Route path='/:page/:task?'>
                    <Inbox />
                  </Route>
                </Route>
                <Route path='/outbox'>
                  <Route path='/:page/:task?'>
                    <Outbox />
                  </Route>
                </Route>
                <Redirect to='/inbox' />
              </Switch>
              <Route path='*/@new-task'>
                <Task />
              </Route>
            </Authorization>
          </Layout>
        </ToastProvider>
      </BrowserRouter>
    </Provider>
  );
}

const AppRedirect = function () {
  const redirect = useSelector(({ layout }) => layout.redirect, shallowEqual)
  const dispatch = useDispatch()

  useEffect(function () {
    if (redirect) {
      dispatch(setRedirect())
    }
  }, [dispatch, redirect])

  return redirect
    ? <Redirect to={redirect.path} push={redirect.push} />
    : null
}

export default App;
