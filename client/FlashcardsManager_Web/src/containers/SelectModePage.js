import React from 'react';

import {connect} from 'react-redux';
import {bindActionCreators} from 'redux';

import * as Ons from 'react-onsenui';

import SelectModeView from '../components/SelectModeView';
import * as Actions from '../actions';
import BasePageWithBackButton from './base/BasePageWithBackButton';


class SelectModePage extends BasePageWithBackButton {
    handleModeSelect = (mode) => {
        const { navigator, actions } = this.props;
        navigator.pushPage({ key: 'LEARNING_FLASHCARD_PAGE' }, { animation: 'flip' });
        actions.startLearning(mode);
    }
    renderPage() {
        return (
            <div>
                <SelectModeView onModeSelect={this.handleModeSelect} />
            </div>
        );
    }
}


const mapDispatchToProps = (dispatch) => {
    return {
      actions: bindActionCreators(Actions, dispatch)
    };
  };

export default connect(null, mapDispatchToProps)(SelectModePage);