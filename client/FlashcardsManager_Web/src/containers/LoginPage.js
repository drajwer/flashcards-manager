import React from 'react';
import { Input, Button, Toolbar, Page, Card } from 'react-onsenui';
import { loginApiCall } from '../api/loginApi';
import { connect } from 'react-redux';
import { receiveAccessToken } from '../actions/login';
import LoginCard from '../components/LoginCard';
import { getAdminCategoriesApiCall } from '../api/adminApi';

class LoginPage extends React.Component {
    constructor(props) {
        super(props);
    }

    sendCredentials = (login, password) => {
        const { navigator } = this.props;
        return loginApiCall(login, password)
            .then(json => {
                this.props.dispatch(receiveAccessToken(json.access_token));
            })
            .then(() => navigator.pushPage({key: 'MAIN_PAGE' }))
    }

    handleRegisterClick = () => {
        const { navigator } = this.props;
        navigator.pushPage({ key: 'REGISTER_PAGE' }, { animation: 'slide' });
        
    }

    render() {
        return (
            <Page contentStyle={{ padding: 50 }} >
                <Toolbar>
                    <p className='center'>Flashcards Manager</p>
                </Toolbar>

                <LoginCard sendCredentials={this.sendCredentials} handleRegisterClick={this.handleRegisterClick} />
            </Page>
        )
    }
}

function mapDispatchToProps(dispatch) {
    return {
        dispatch
    };
}
export default connect(null, mapDispatchToProps)(LoginPage);