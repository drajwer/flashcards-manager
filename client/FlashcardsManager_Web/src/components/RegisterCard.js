import React from 'react';
import { Input, Button, Toolbar, Page, Card } from 'react-onsenui';
import FetchingView from '../components/FetchingView';

export default class RegisterCard extends React.Component {
    constructor(props) {
        super(props);
        this.state = { username: '', password: '', confirmPassword: '', name: '', surname: '', errorVisible: false, isFetching: false };
    }
    handleUsernameChange = (e) => {
        this.setState({ username: e.target.value });
    }

    handlePasswordChange = (e) => {
        this.setState({ password: e.target.value });
    }

    handleNameChange = (e) => {
        this.setState({ name: e.target.value });
    }

    handleSurnameChange = (e) => {
        this.setState({ surname: e.target.value });
    }

    handleConfirmPasswordChange = (e) => {
        this.setState({ confirmPassword: e.target.value });
    }

    handleRegisterClick = () => {
        this.setState({ isFetching: true });
        this.props.sendCredentials(this.state.username, this.state.name, this.state.surname,
            this.state.password, this.state.confirmPassword)
            .catch(err => {
                console.log(err);
                this.setState({ errorVisible: true, isFetching: false });
            });
    }

    disableButton() {
        return !(this.state.username.length > 0 && this.state.password.length > 0 && this.state.password === this.state.confirmPassword);
    }
    render() {
        const errorMessage = this.state.errorVisible ? 'Nazwa użytkownika jest już zajęta lub hasło jest zbyt proste' : '';
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
                                    value={this.state.username}
                                    onChange={this.handleUsernameChange}
                                    modifier='underbar'
                                    float
                                    placeholder='Nazwa użytkownika' />
                            </p>
                            <p >
                                <Input
                                    value={this.state.name}
                                    onChange={this.handleNameChange}
                                    modifier='underbar'
                                    float
                                    placeholder='Imię' />
                            </p>
                            <p >
                                <Input
                                    value={this.state.surname}
                                    onChange={this.handleSurnameChange}
                                    modifier='underbar'
                                    float
                                    placeholder='Nazwisko' />
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
                                <Input
                                    value={this.state.confirmPassword}
                                    onChange={this.handleConfirmPasswordChange}
                                    modifier='underbar'
                                    type='password'
                                    float
                                    placeholder='Potwierdź hasło' />
                            </p>
                            <p>
                                <Button onClick={this.handleRegisterClick} disabled={this.disableButton()}>Stwórz konto</Button>
                            </p>
                        </div>
                }
            </div>
        )
    }
}
