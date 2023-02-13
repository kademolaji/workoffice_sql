import axios from 'axios';

export const loginUserAccount = (email, password) => {
  const data = {
    userName: email,
    password,
  };

  return axios.post(
    'https://localhost:44344/api/useraccount/authenticate',
    data
  );
};

export const sendPasswordResetEmail = (email) => {
  const data = {
    email
  };

  return axios.post(
    'https://localhost:44344/api/useraccount/forgot-password',
    data
  );
};

export const confirmPasswordReset = (token, password, confirmPassword) => {
  const data = {
    token,
    password,
    confirmPassword
  };

  return axios.post(
    'https://localhost:44344/api/useraccount/reset-password',
    data
  );
};



export const createUserWithEmailAndPassword = (data) => {

  return axios.post(
    'https://localhost:44344/api/useraccount/register',
    data
  );
};

export const signOut = () => {

  return axios.post(
    'https://localhost:44344/api/useraccount/register',
    {}
  );
};
