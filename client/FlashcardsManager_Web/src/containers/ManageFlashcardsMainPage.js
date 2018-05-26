import React from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import * as Ons from 'react-onsenui';
import ons from 'onsenui';

import FlashcardsListView from '../components/FlashcardsListView';
import ErrorView from '../components/ErrorView';
import FetchingView from '../components/FetchingView';

import * as Actions from '../actions/manageFlashcards';
import BasePageWithBackButton from './base/BasePageWithBackButton';


class ManageFlashcardsMainPage extends BasePageWithBackButton {
    componentDidMount() {
        const { flashcards, isInvalid } = this.props.manageFlashcards;
        const categoryId = this.props.categoryId;
        if (isInvalid) {
            return;
        }
        this.props.actions.getOwnFlashcards(categoryId);
    }
    handleFlashcardSelect = (flashcardId) => {
        const { navigator, actions } = this.props;

        navigator.pushPage({ key: 'MANAGE_FLASHCARD_PAGE' }, { animation: 'slide' });
        actions.selectFlashcard(flashcardId);
    }
    handleAddClick = (key, value, keyDesc, valueDesc) => {
        const { addFlashcard } = this.props.actions;
        addFlashcard(key, value, keyDesc, valueDesc);
    }

    handleDismissClick = () => {
        const { resetToast } = this.props.actions;
        resetToast();
    }
    renderPage() {
        const { flashcards, isFetching, isInvalid, showToast, message } = this.props.manageFlashcards;
        if (isInvalid) {
            return <ErrorView />;
        };
        return (
            <div>
                {
                    isFetching
                        ? <FetchingView />

                        : <FlashcardsListView
                            onAddButtonClick={this.handleAddClick}
                            title="Twoje fiszki"
                            onFlashcardSelect={this.handleFlashcardSelect}
                            flashcards={flashcards}
                            showToast={showToast}
                            message={message}
                            onDismissClick={this.handleDismissClick}
                            onAddButtonClick={this.handleAddClick}
                        />
                }
            </div>
        )
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(Actions, dispatch)
    };
};

const mapStateToProps = (state) => ({
    manageFlashcards: state.manageFlashcards,
    categoryId: state.manageCategories.categoryId
});

export default connect(mapStateToProps, mapDispatchToProps)(ManageFlashcardsMainPage);