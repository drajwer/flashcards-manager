import React from 'react';

import * as Ons from 'react-onsenui';

import NavBar from '../../components/NavBar';
import { routeLabels } from '../../routing/routeLabels';

export default class BasePageWithBackButton extends React.Component {
    constructor() {
        super();
        this.state = { isOpen: false };
    }
    
    render() {
        const { navigator, title } = this.props;
        return (

            <Ons.Page renderToolbar={() => <NavBar title={title} onButtonClick={this.handleBackButtonClick} backButton/>}>
                {this.renderPage()}
            </Ons.Page>
        );
    }
}
