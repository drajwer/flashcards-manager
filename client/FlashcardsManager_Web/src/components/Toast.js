import React from 'react';
import * as Ons from 'react-onsenui';

export default class Toast extends React.Component {
    render() {
        return (
            <Ons.Toast isOpen={this.props.showToast}>
                <div className="message">
                    {this.props.message}
                </div>
                <button onClick={this.props.onDismissClick}>
                    Zamknij
                </button>
            </Ons.Toast>
        )
    }
}