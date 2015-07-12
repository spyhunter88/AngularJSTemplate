app.constant('AUTH_EVENTS', {
    loginSuccess: 'auth-login-success',
    loginFailed: 'auth-login-failed',
    logoutSuccess: 'auth-logout-success',
    sessionTimeout: 'auth-session-timeout',
    notAuthenticated: 'auth-not-authenticated',
    notAuthorized: 'auth-not-authorized',
    refreshingToken: 'auth-refreshing-token'
})
.constant('USER_ROLES', {
    admin: 'admin_role',
    user: 'user_role',
    public: 'public_role'
});