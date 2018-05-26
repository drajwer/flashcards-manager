import React from 'react';
import { Input, Button, Toolbar, Page, Card } from 'react-onsenui';
import { registerApiCall } from '../api/loginApi';
import { connect } from 'react-redux';
import { receiveAccessToken } from '../actions/login';
import RegisterCard from '../components/RegisterCard';
import BasePageWithBackButton from './base/BasePageWithBackButton';

class RegisterPage extends BasePageWithBackButton {
    constructor(props) {
        super(props);
    }

    sendCredentials = (username, name, surname, password, confirmPassword) => {
        const { navigator } = this.props;
        return registerApiCall(username, name, surname, password, confirmPassword)
            .then(() => navigator.popPage());

    }
    renderPage() {
        return (
            <RegisterCard sendCredentials={this.sendCredentials} />
        )
    }
}

function mapDispatchToProps(dispatch) {
    return {
        dispatch
    };
}
export default connect(null, mapDispatchToProps)(RegisterPage);