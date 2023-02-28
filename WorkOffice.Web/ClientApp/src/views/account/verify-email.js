// import React, {  useEffect } from 'react';
// import { Row, Card, CardTitle } from 'reactstrap';
// import { NavLink } from 'react-router-dom';
// import { connect } from 'react-redux';
// import { Colxx } from 'components/common/CustomBootstrap';
// import IntlMessages from 'helpers/IntlMessages';
// import { forgotPassword } from 'redux/actions';


// const VerifyEmail = ({
// location,
 
// }) => {

//     const onVerifyEmail = () => {
//           const params = new URLSearchParams(location.search);
//           const resetToken = params.get('token');
//           if (resetToken) {
//             if (values.password !== '') {
//               resetPasswordAction({
//                 token: resetToken,
//               });
//             }
        
//         }
//       };
    

//   useEffect(() => {
    
//   }, [error, forgotUserMail, loading]);


//   return (
//     <Row className="h-100">
//       <Colxx xxs="12" md="10" className="mx-auto my-auto">
//         <Card className="auth-card">
//           <div className="position-relative image-side ">
//             <p className="text-white h2">MAGIC IS IN THE DETAILS</p>
//             <p className="white mb-0">
//               Please use your e-mail to reset your password. <br />
//               If you are not a member, please{' '}
//               <NavLink to="/account/register" className="white">
//                 register
//               </NavLink>
//               .
//             </p>
//           </div>
//           <div className="form-side">
//             <NavLink to="/" className="white">
//               <span className="logo-single" />
//             </NavLink>
//             <CardTitle className="mb-4">
//               <IntlMessages id="user.forgot-password" />
//             </CardTitle>

          
//           </div>
//         </Card>
//       </Colxx>
//     </Row>
//   );
// };

// const mapStateToProps = ({ authUser }) => {
//   const { forgotUserMail, loading, error } = authUser;
//   return { forgotUserMail, loading, error };
// };

// export default connect(mapStateToProps, {
//   forgotPasswordAction: forgotPassword,
// })(VerifyEmail);
