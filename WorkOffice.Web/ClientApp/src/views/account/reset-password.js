import React, { useState, useEffect } from 'react';
import { Row, Card, CardTitle, Label, FormGroup, Button } from 'reactstrap';
import { NavLink } from 'react-router-dom';
import { Formik, Form, Field } from 'formik';
import { connect } from 'react-redux';
import { Colxx } from 'components/common/CustomBootstrap';
import IntlMessages from 'helpers/IntlMessages';
import { resetPassword } from 'redux/actions';
import { NotificationManager } from 'components/common/react-notifications';

const validateNewPassword = (values) => {
  const { newPassword, confirmNewPassword } = values;
  const errors = {};
  if (confirmNewPassword && newPassword !== confirmNewPassword) {
    errors.confirmNewPassword = 'Please check your new password';
  }
  return errors;
};

const ResetPassword = ({ location, loading, error, resetPasswordAction }) => {
  const [newPassword] = useState('');
  const [confirmNewPassword] = useState('');

  useEffect(() => {
    if (error) {
      NotificationManager.warning(
        error,
        'Forgot Password Error',
        3000,
        null,
        null,
        ''
      );
    } else if (!loading && newPassword === 'success')
      NotificationManager.success(
        'Please login with your new password.',
        'Reset Password Success',
        3000,
        null,
        null,
        ''
      );
  }, [error, loading, newPassword]);

  const onResetPassword = (values) => {
    if (!loading) {
      const params = new URLSearchParams(location.search);
      const resetToken = params.get('token');
      if (resetToken) {
        if (values.password !== '') {
          resetPasswordAction({
            newPassword: values.newPassword,
            confirmNewPassword: values.confirmNewPassword,
            token: resetToken,
          });
        }
      } else {
        NotificationManager.warning(
          'Please check your email url.',
          'Reset Password Error',
          3000,
          null,
          null,
          ''
        );
      }
    }
  };

  const initialValues = { newPassword, confirmNewPassword };

  return (
    <Row className="h-100">
      <Colxx xxs="12" md="10" className="mx-auto my-auto">
        <Card className="auth-card">
          <div className="position-relative image-side ">
            <p className="text-white h2">MAGIC IS IN THE DETAILS</p>
            <p className="white mb-0">
              Please use your e-mail to reset your password. <br />
              If you are not a member, please{' '}
              <NavLink to="/account/register" className="white">
                register
              </NavLink>
              .
            </p>
          </div>
          <div className="form-side">
            <NavLink to="/" className="white">
              <span className="logo-single" />
            </NavLink>
            <CardTitle className="mb-4">
              <IntlMessages id="user.reset-password" />
            </CardTitle>

            <Formik
              validate={validateNewPassword}
              initialValues={initialValues}
              onSubmit={onResetPassword}
            >
              {({ errors, touched }) => (
                <Form className="av-tooltip tooltip-label-bottom">
                  <FormGroup className="form-group has-float-label">
                    <Label>
                      <IntlMessages id="user.new-password" />
                    </Label>
                    <Field
                      className="form-control"
                      name="newPassword"
                      type="password"
                    />
                  </FormGroup>
                  <FormGroup className="form-group has-float-label">
                    <Label>
                      <IntlMessages id="user.new-password-again" />
                    </Label>
                    <Field
                      className="form-control"
                      name="confirmNewPassword"
                      type="password"
                    />
                    {errors.confirmNewPassword && touched.confirmNewPassword && (
                      <div className="invalid-feedback d-block">
                        {errors.confirmNewPassword}
                      </div>
                    )}
                  </FormGroup>

                  <div className="d-flex justify-content-between align-items-center">
                    <NavLink to="/account/login">
                      <IntlMessages id="user.login-title" />
                    </NavLink>
                    <Button
                      color="primary"
                      className={`btn-shadow btn-multiple-state ${loading ? 'show-spinner' : ''
                        }`}
                      size="lg"
                    >
                      <span className="spinner d-inline-block">
                        <span className="bounce1" />
                        <span className="bounce2" />
                        <span className="bounce3" />
                      </span>
                      <span className="label">
                        <IntlMessages id="user.reset-password-button" />
                      </span>
                    </Button>
                  </div>
                </Form>
              )}
            </Formik>
          </div>
        </Card>
      </Colxx>
    </Row>
  );
};

const mapStateToProps = ({ authUser }) => {
  const { newPassword, confirmNewPassword, token, loading, error } = authUser;
  return { newPassword, confirmNewPassword, token, loading, error };
};

export default connect(mapStateToProps, {
  resetPasswordAction: resetPassword,
})(ResetPassword);
