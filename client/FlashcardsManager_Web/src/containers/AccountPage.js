import React from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import * as Ons from 'react-onsenui';
import ons from 'onsenui';

import BasePageWithMenu from './base/BasePageWithMenu';
import ErrorView from '../components/ErrorView';
import FetchingView from '../components/FetchingView';

import * as Actions from '../actions/login';
import { resetState } from '../actions/index';



class AccountPage extends BasePageWithMenu {
    constructor() {
        super();
    }
    resetState() {
        this.props.resetState();
    }

    logout = () => {
        const { logout } = this.props.actions;
        const { navigator } = this.props;
        logout();
        navigator.resetPage({ key: 'LOGIN_PAGE' }, { animation: 'fade' });
    }

    renderPage() {
        return (
                <div className="card" >
                    <div style={{ textAlign: 'center', padding: 30 }} >
                        <Ons.Button onClick={this.logout} >Wyloguj</Ons.Button>
                    </div>
                </div>
        );
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(Actions, dispatch),
        resetState: bindActionCreators(resetState, dispatch)
    };
};

const mapStateToProps = (state) => ({
});

export default connect(mapStateToProps, mapDispatchToProps)(AccountPage);