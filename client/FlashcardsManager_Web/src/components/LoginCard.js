import React from 'react';
import { Input, Button, Toolbar, Page, Card } from 'react-onsenui';
import FetchingView from '../components/FetchingView';

export default class LoginCard extends React.Component {
    constructor(props) {
        super(props);
        this.state = { login: '', password: '', errorVisible: false, isFetching: false };
    }
    handleUsernameChange = (e) => {
        this.setState({ login: e.target.value });
    }

    handlePasswordChange = (e) => {
        this.setState({ password: e.target.value });
    }

    handleClick = () => {
        this.setState({ isFetching: true });
        this.props.sendCredentials(this.state.login, this.state.password)
            .then(() => { this.setState({ errorVisible: false }) })
            .catch(error => {
                console.log(error);
                this.setState({ errorVisible: true, isFetching: false });
            });
    }

    render() {
        const errorMessage = this.state.errorVisible ? 'Błędna nazwa użytkownika lub hasło' : '';
        return (

            <div className="card" >
                {
                    this.state.isFetching ?
                        <FetchingView />
                        :
                        <div style={{ textAlign: 'center' }}>
                            <p style={{ color: 'red' }}>
                                {errorMessage}
                            </p>
                            <p >
                                <Input
                                    value={this.state.login}
                                    onChange={this.handleUsernameChange}
                                    modifier='underbar'
                                    float
                                    placeholder='Nazwa użytkownika' />
                            </p>
                            <p>
                                <Input
                                    value={this.state.password}
                                    onChange={this.handlePasswordChange}
                                    modifier='underbar'
                                    type='password'
                                    float
                                    placeholder='Hasło' />
                            </p>
                            <p>
                                <Button onClick={this.handleClick}>Zaloguj się</Button>
                            </p>
                            <p>
                                <Button onClick={this.props.handleRegisterClick}>Zarejestruj się</Button>
                            </p>
                        </div>
                }
            </div>
        )
    }
}
