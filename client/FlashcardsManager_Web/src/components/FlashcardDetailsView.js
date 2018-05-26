import React from 'react';
import ons from 'onsenui';
import * as Ons from 'react-onsenui';
import ReactCardFlip from 'react-card-flip';

import NavBar from './NavBar';
import LocationList from '../containers/LocationList';
import AddLocation from '../containers/AddLocation';
import FlashcardView from './FlashcardView';

import { flashcardResults } from '../api/types';

export default class FlashcardDetailsView extends React.Component {
    constructor() {
        super();
        this.state = { isFlipped: false };
    }
    handleFilpClick = () => {
        const isFlipped = this.state.isFlipped;
        this.setState({ isFlipped: !isFlipped });
    }
    render() {
        const { isFlipped } = this.state;
        const buttonStyle = { margin: '5px' };
        return (
            <div>
                <Ons.Page>
                        <ReactCardFlip isFlipped={this.state.isFlipped}>
                            <div key="front">
                                {this.renderFrontFlashcard()}
                            </div>

                            <div key="back">
                                {this.renderBackFlashcard()}
                            </div>
                        </ReactCardFlip>
                    <div style={{ textAlign: 'center', margin: '300px 0 0 0' }}>
                        <Ons.Button onClick={this.handleFilpClick}>Odwróć fiszkę</Ons.Button>
                    </div>
                </Ons.Page>
            </div>
        );
    }
    renderFrontFlashcard() {
        const { flashcard } = this.props;
        return (
            <FlashcardView value={flashcard.key} description={flashcard.keyDescription} />
        );
    }

    renderBackFlashcard() {
        const { flashcard } = this.props;
        if (!this.state.isFlipped) {
            return null;
        }
        return (
            <FlashcardView value={flashcard.value} description={flashcard.valueDescription} />
        );
    }
}