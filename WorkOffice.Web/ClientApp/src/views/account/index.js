import React, { Suspense } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import AccountLayout from 'layout/AccountLayout';

const Login = React.lazy(() =>
  import(/* webpackChunkName: "account-login" */ './login')
);
const Register = React.lazy(() =>
  import(/* webpackChunkName: "account-register" */ './register')
);
const ForgotPassword = React.lazy(() =>
  import(/* webpackChunkName: "account-forgot-password" */ './forgot-password')
);
const ResetPassword = React.lazy(() =>
  import(/* webpackChunkName: "account-reset-password" */ './reset-password')
);

const VerifyEmail = React.lazy(() =>
  import(/* webpackChunkName: "account-verify-email" */ './verify-email')
);

const Account = ({ match }) => {
  return (
    <AccountLayout>
      <Suspense fallback={<div className="loading" />}>
        <Switch>
          <Redirect exact from={`${match.url}/`} to={`${match.url}/login`} />
          <Route
            path={`${match.url}/login`}
            render={(props) => <Login {...props} />}
          />
          <Route
            path={`${match.url}/register`}
            render={(props) => <Register {...props} />}
          />
          <Route
            path={`${match.url}/forgot-password`}
            render={(props) => <ForgotPassword {...props} />}
          />
          <Route
            path={`${match.url}/reset-password`}
            render={(props) => <ResetPassword {...props} />}
          />
            <Route
            path={`${match.url}/verify-email`}
            render={(props) => <VerifyEmail {...props} />}
          />
          <Redirect to="/error" />
        </Switch>
      </Suspense>
    </AccountLayout>
  );
};

export default Account;
