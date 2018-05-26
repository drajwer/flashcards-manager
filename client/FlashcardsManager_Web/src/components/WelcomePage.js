import React from 'react';
import { Input, Button, Toolbar, Page } from 'react-onsenui';

import LoginCard from './LoginCard';

export default class WelcomePage extends React.Component {
    render() {
        return (
            <div>
                <Page renderToolbar={this.renderToolbar}>
                    <section style={{ textAlign: 'center' }}>
                        <p>Zaloguj się i rozpocznij naukę!</p>
                        <LoginCard/>
                        <p>
                            <Button onClick={this.handleClick}>Zarejestruj się</Button>
                        </p>
                    </section>
                </Page>
            </div>
        );
    }

    renderToolbar() {
        return (
            <Toolbar>
                <div className='center'>Fiszki</div>
            </Toolbar>
        );
    }
}